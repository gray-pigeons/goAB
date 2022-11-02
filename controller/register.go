package controller

import (
	"fmt"
	data "goAB/database"

	"github.com/gin-gonic/gin"
)

func init() {
	controllerMap["/register"] = register
}

type registerReq struct {
	Username string `json:"username,omitempty"`
	Password string `json:"password,omitempty"`
}

type registerRsp struct {
	Code  string `json:"code,omitempty"`
	Error string `json:"error,omitempty"`
}

// 注册
func register(ctx *gin.Context) {

	req := new(registerReq)
	fmt.Println("register-------", req.Username, req.Password, rspState)

	// checkLength()
	if rspState["Code"] != "" {
		ctx.JSON(401, rspState)
		return
	}

	AddUser(req.Username, req.Password, ctx)

}

func AddUser(username string, password string, ctx *gin.Context) {

	rspState["Code"], rspState["text"] = "", ""
	res2, err2 := data.DBRead.Exec("insert into `user`(name,pass)value(?,?);", username, password)
	fmt.Println("addUser结果=", res2, err2)
	if err2 != nil {
		rspState["Code"] = err2.Error()
		rspState["text"] = "添加用户失败,用户名重复"
		ctx.JSON(200, rspState)
		return
	}

	rspState["Code"] = "10001"
	rspState["text"] = "添加用户成功"
	ctx.JSON(200, rspState)
}
