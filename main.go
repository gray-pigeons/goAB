package main

import (
	"fmt"
	data "goAB/data/db"
	"net/http"

	"github.com/gin-gonic/gin"
	_ "github.com/go-sql-driver/mysql"
)

func main() {
	data.InitDB()
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

// 注册
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
	isExist, err := data.IsExist(username)
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
