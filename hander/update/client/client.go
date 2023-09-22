package updateclient

import (
	"github.com/gin-gonic/gin"
)

var zipFilePath = "hander/download/zip"

func MasterClient(c *gin.Context) {
	c.Header("Content-Type", "application/octet-stream")
	c.Header("Content-Disposition", "attachment; filename=Master.zip")
	c.Header("Content-Transfer-Encoding", "binary")
	c.File(zipFilePath + "/Master.zip")
}
func AutoUpdaterClient(c *gin.Context) {
	c.Header("Content-Type", "application/octet-stream")
	c.Header("Content-Disposition", "attachment; filename=AutoUpdate.zip")
	c.Header("Content-Transfer-Encoding", "binary")
	c.File(zipFilePath + "/AutoUpdate.zip")
}
