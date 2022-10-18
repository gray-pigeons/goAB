package main

import (
	"fmt"
	controller "goAB/controller"
	data "goAB/database"
	"net/http"

	"github.com/gin-gonic/gin"
	_ "github.com/go-sql-driver/mysql"
)

func main() {
	// go server.InitTcpServer()
	fmt.Println("1111-------------------")
	data.InitDB()
	// go client.InitClient()
	router := gin.Default()
	router.GET("/test", func(ctx *gin.Context) {
		ctx.JSON(200, "嘿嘿嘿嘿")
	})
	router.StaticFS("/AssetBundle", http.Dir("AssetBundle"))
	router.StaticFS("/FixHot", http.Dir("FixHot"))

	controller.Init(router)

	err := router.Run()
	if err != nil {
		fmt.Println(err)
	}
}
