package updatexml

import "github.com/gin-gonic/gin"

var xmlFilePath = "hander/download/xml"

func AutoUpdaterXml(c *gin.Context) {
	c.Header("Content-Type", "text/xml")
	c.Header("Content-Disposition", "attachment; filename=AutoUpdate.xml")
	c.Header("Content-Transfer-Encoding", "binary")
	c.File(xmlFilePath + "/AutoUpdate.xml")
}

func UpdateMasterXml(c *gin.Context) {
	c.Header("Content-Type", "text/xml")
	c.Header("Content-Disposition", "attachment; filename=UpdateMaster.xml")
	c.Header("Content-Transfer-Encoding", "binary")
	c.File(xmlFilePath + "/UpdateMaster.xml")
}
