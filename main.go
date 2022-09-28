package main

import (
	controller "goAB/controller"
	data "goAB/database"
	"log"
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

	controller.Init(router)

	err := router.Run()
	if err != nil {
		// fmt.Println(err)
		log.Default().Fatalln(err)
	}
}
