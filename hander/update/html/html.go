package updatehtml

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

func AutoUpdaterHtml(c *gin.Context) {
	c.HTML(http.StatusOK, "AutoUpdater.html", gin.H{
		"title": "王九",
	})
}

func UpdateMasterHtml(c *gin.Context) {
	c.HTML(http.StatusOK, "UpdateMaster.html", gin.H{
		"title": "王九",
	})
}

// func fileExists(filePath string) bool {
// 	_, err := os.Stat(filePath)
// 	if err == nil {
// 		return true // 文件存在
// 	}
// 	if os.IsNotExist(err) {
// 		return false // 文件不存在
// 	}
// 	return false // 出现了其他错误，无法确定文件是否存在
// }
