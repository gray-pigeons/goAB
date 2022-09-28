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

}

func AddUser(username string, password string) {

	handler := data.DBRead.QueryRow("select count(*) from user where name=?", username)
	if handler.Err() != nil {

	}
	data.DBRead.Exec("")
}
