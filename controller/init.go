package controller

import (
	"github.com/gin-gonic/gin"
)

var controllerMap = make(map[string]func(*gin.Context))
var rspState = make(map[string]string)

type User struct {
	Id       int
	Username string
	Password string
}

// 对所有请求进行初始化
// go 中 init 初始化先后顺序 https://learnku.com/go/t/47135
func Init(router *gin.Engine) {
	for url, hander := range controllerMap {
		router.POST(url, hander)
	}
}
