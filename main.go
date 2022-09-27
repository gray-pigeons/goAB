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
	router.Run()
}

type User struct {
	Id       int
	Username string `json:"username"`
	Password string `json:"password"`
}

// 登录
func login(ctx *gin.Context) {

	username := ctx.Request.FormValue("username")
	password := ctx.Request.FormValue("password")
	rspState := make(map[string]string)

	fmt.Println("1-------", username, password, rspState)

	if len(username) < 6 || len(username) > 16 {
		rspState["state"] = "1001"
		rspState["text"] = "用户名长度要在6-16位字符之间"
		ctx.JSON(401, rspState)
		return
	}

	if len(password) < 6 || len(password) > 16 {
		rspState["state"] = "1002"
		rspState["text"] = "密码长度要在6-16位之间"
		ctx.JSON(401, rspState)
		return
	}

	//判断用户名是否存在
	isExist := IsExist(username)
	if isExist != nil {
		rspState["state"] = "1003"
		rspState["text"] = "用户名不存在"
		ctx.JSON(401, rspState)
		return
	}

	//判断用户名密码是否正确
	isRight := IsRight(username, password)
	if isRight != nil {
		rspState["state"] = "1003"
		rspState["text"] = "用户名或密码错误"
		ctx.JSON(401, rspState)
		return
	}

	rspState["state"] = "1004"
	rspState["text"] = "登录成功"
	ctx.JSON(200, rspState)
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

// 查看用户是否存在
func IsExist(name string) error {
	row, err := dbRead.Query("select * from user where name=?;", name)
	fmt.Println("1----", row, err)
	fmt.Println(row.Scan(name))
	fmt.Println(err == nil)
	return err
}

// 查看用户密码是否正确
func IsRight(name string, pass string) error {
	row, err := dbRead.Query("select * from user where name=? and pass=?;", name, pass)
	fmt.Println("2----", row, err)
	return err
}
