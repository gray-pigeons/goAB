package controller

import (
	"database/sql"
	"fmt"
	data "goAB/database"

	"github.com/gin-gonic/gin"
)

// https://learnku.com/go/t/47135
func init() {
	controllerMap["/login"] = login
}

type loginReq struct {
	Username string `json:"username,omitempty"`
	Password string `json:"password,omitempty"`
}

type loginRsp struct {
	Code  int    `json:"code,omitempty"`
	Error string `json:"error,omitempty"`
}

// 登录
func login(ctx *gin.Context) {

	var req = new(loginReq)
	req.Username = ""
	req.Password = ""
	err := ctx.BindJSON(req)
	// username := ctx.Request.FormValue("username")
	// password := ctx.Request.FormValue("password")
	if err != nil {
		ctx.JSON(400, nil)
		return
	}

	fmt.Println("login-------1: \n", req.Username, req.Password, rspState)

	if !req.checkLength() {
		ctx.JSON(405, nil)
		return
	}

	//查找用户名密码是否正确
	row, err := data.DBRead.Query("select * from user where name=?;", req.Username)
	if err != nil {
		fmt.Println("查询用户出错:", err)

		if err == sql.ErrNoRows {
			ctx.JSON(200, &loginRsp{
				Code:  -1,
				Error: "用户不存在",
			})
		} else {
			ctx.JSON(200, &loginRsp{
				Code:  -1,
				Error: "系统错误, 请检查平台系统",
			})
		}
		return
	}

	for row.Next() {
		err2 := row.Scan(&id_, &name_, &pass_)
		if err2 != nil {
			fmt.Println("密码错误:", err2)
			ctx.JSON(200, &loginRsp{
				Code:  -1,
				Error: "密码错误",
			})
			return
		}
	}
	defer row.Close()
	fmt.Println("login查找结果:", id_, name_, pass_)

	if pass_ != req.Password {
		ctx.JSON(200, &loginRsp{
			Code:  -1,
			Error: "用户名不存在",
		})
		return
	}

	if pass_ != req.Password {
		ctx.JSON(200, &loginRsp{
			Code:  -1,
			Error: "密码错误",
		})
		return
	}

	fmt.Println(name_, pass_)

	ctx.JSON(200, &loginRsp{
		Code:  0,
		Error: "",
	})
}

func (this_ *loginReq) checkLength() bool {
	if len(this_.Username) < 6 || len(this_.Username) > 16 {
		return false
	}

	if len(this_.Password) < 6 || len(this_.Password) > 16 {
		return false
	}
	return true
}

var (
	id_          int
	name_, pass_ string
)
