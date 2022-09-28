package controller

import (
	"fmt"
	data "goAB/database"

	"github.com/gin-gonic/gin"
)

func init() {
	controllerMap["/register"] = register
}

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

	AddUser(username, password, ctx)

}

func AddUser(username string, password string, ctx *gin.Context) {

	rspState["state"], rspState["text"] = "", ""
	res2, err2 := data.DBRead.Exec("insert into `user`(name,pass)value(?,?);", username, password)
	fmt.Println("addUser结果=", res2, err2)
	if err2 != nil {
		rspState["state"] = err2.Error()
		rspState["text"] = "添加用户失败,用户名重复"
		ctx.JSON(200, rspState)
		return
	}

	rspState["state"] = "10001"
	rspState["text"] = "添加用户成功"
	ctx.JSON(200, rspState)
}
