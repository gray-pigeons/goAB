package controller

import (
	"fmt"
	data "goAB/database"

	"github.com/gin-gonic/gin"
)

// https://learnku.com/go/t/47135
func init() {
	controllerMap["/login"] = login
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
	isExist, err := selectUser(username)
	if err != nil {
		ctx.JSON(500, "出错了")
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

var (
	id_          int
	name_, pass_ string
)

// 查看用户和密码是否正确
func selectUser(name string) (User, error) {
	row, err := data.DBRead.Query("select * from user where name=?;", name)
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
