package main

import (
	"fmt"
	controller "goAB/controller"
	updateclient "goAB/hander/update/client"
	updatehtml "goAB/hander/update/html"
	updatexml "goAB/hander/update/xml"
	"goAB/server"
	"net/http"

	"github.com/gin-gonic/gin"
	_ "github.com/go-sql-driver/mysql"
)

func main() {
	go server.InitTcpServer()
	fmt.Println("1111-------------------")
	//data.InitDB()
	// go client.InitClient()
	router := gin.New()
	router.StaticFS("/AssetBundle", http.Dir("AssetBundle"))
	router.StaticFS("/FixHot", http.Dir("FixHot"))

	router.GET("/test", func(ctx *gin.Context) {
		ctx.JSON(200, "嘿嘿嘿嘿")
	})

	// 1 是给客户端的，2和3是在客户端拿到xml文件后发送说明请求和下载请求用的
	//1. 先下载版本文件xml
	xmlURL := "/download/xml"
	router.GET(xmlURL+"/autoupdaterxml", updatexml.AutoUpdaterXml)
	router.GET(xmlURL+"/updatemasterxml", updatexml.UpdateMasterXml)

	//2. 然后下载更新说明html
	viewsURL := "/views"
	router.LoadHTMLGlob("templates/*") // 这里指定模板文件所在的目录
	router.GET(viewsURL+"/autoupdater", updatehtml.AutoUpdaterHtml)
	router.GET(viewsURL+"/updatemaster", updatehtml.UpdateMasterHtml)

	//3. 最后再下载压缩包zip
	zipURL := "/download/zip"
	router.GET(zipURL+"/autoupdater", updateclient.AutoUpdaterClient)
	router.GET(zipURL+"/updatemaster", updateclient.MasterClient)

	controller.Init(router)

	err := router.Run()
	if err != nil {
		fmt.Println(err)
	}
}
