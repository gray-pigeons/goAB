package main

import (
	"database/sql"
	"fmt"
	"net/http"

	"github.com/gin-gonic/gin"
	_ "github.com/go-sql-driver/mysql"
)

func main() {
	initDB()
	router := gin.Default()
	router.GET("/test", func(ctx *gin.Context) {
		ctx.JSON(200, "嘿嘿嘿嘿")
	})
	router.StaticFS("/AssetBundle", http.Dir("AssetBundle"))
	router.StaticFS("/FixHot", http.Dir("FixHot"))

	router.POST("/login", login)
	router.PUT("/register", register)
	router.Run()
}

type User struct {
	Id       int
	Username string
	Password string
}

var rspState = make(map[string]string)

func register(ctx *gin.Context) {
	username := ctx.Request.FormValue("username")
	password := ctx.Request.FormValue("password")
	fmt.Println("register-------", username, password, rspState)

	checkLength(username, password)
	if rspState["state"] != "" {
		ctx.JSON(401, rspState)
		return
	}
}

// 登录
func login(ctx *gin.Context) {

	username := ctx.Request.FormValue("username")
	password := ctx.Request.FormValue("password")

	fmt.Println("login-------", username, password, rspState)

	checkLength(username, password)
	if rspState["state"] != "" {
		ctx.JSON(401, rspState)
		return
	}
	//判断用户名是否存在
	isExist, err := IsExist(username)
	if err != nil {
		ctx.JSON(404, "出错了")
		return
	}
	if isExist.Username != username {
		rspState["state"] = "1003"
		rspState["text"] = "用户名不存在"
		ctx.JSON(401, rspState)
		return
	}

	if isExist.Password != password {
		rspState["state"] = "1003"
		rspState["text"] = "密码错误"
		ctx.JSON(401, rspState)
		return
	}

	fmt.Println(isExist.Username, isExist.Password)
	rspState["state"] = "1004"
	rspState["text"] = "登录成功"
	ctx.JSON(200, rspState)
}

func checkLength(username string, password string) {
	rspState["state"], rspState["text"] = "", ""
	if len(username) < 6 || len(username) > 16 {
		rspState["state"] = "1001"
		rspState["text"] = "用户名长度要在6-16位字符之间"
		return
	}

	if len(password) < 6 || len(password) > 16 {
		rspState["state"] = "1002"
		rspState["text"] = "密码长度要在6-16位之间"
		return
	}
}

//-------连接数据库----------------

var dbRead *sql.DB

// var dbWrite *sql.DB

const (
	driveName  = "mysql"
	dbUser     = "root"
	dbPass     = "root"
	dbProtocol = "tcp"
	dbAddress  = "127.0.0.1:3306"
	dbName     = "go_ab"
)

func initDB() {
	//数据源名
	conStr := dbUser + ":" + dbPass + "@" + dbProtocol + "(" + dbAddress + ")" + "/" + dbName
	dbSQL, err := sql.Open(driveName, conStr)
	if err != nil {
		fmt.Println("mysql open is warn :", err)
		return
	}
	//数据库设置
	dbSQL.SetConnMaxLifetime(60)
	dbSQL.SetConnMaxIdleTime(60)
	dbSQL.SetMaxOpenConns(10)
	dbSQL.SetMaxIdleConns(10)

	//连接测试
	err2 := dbSQL.Ping()
	if err2 != nil {
		fmt.Println("mysql is connect ping failed :", err2)
		return
	}
	dbRead = dbSQL
	fmt.Println("mysql is connect success")
}

var (
	id_          int
	name_, pass_ string
)

// 查看用户和密码是否正确
func IsExist(name string) (User, error) {
	row, err := dbRead.Query("select * from user where name=?;", name)
	if err != nil {
		fmt.Println("查询用户出错:", err)
		return User{}, err
	}

	for row.Next() {
		err2 := row.Scan(&id_, &name_, &pass_)
		if err2 != nil {
			fmt.Println("解析用户名和密码发生错误:", err2)
			return User{}, err2
		}

		fmt.Println("查找结果:", id_, name_, pass_)
	}
	defer row.Close()
	return User{id_, name_, pass_}, nil
}
