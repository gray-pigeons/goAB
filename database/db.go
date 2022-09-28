package data

import (
	"database/sql"
	"fmt"
)

//-------连接数据库----------------

var DBRead *sql.DB

// var dbWrite *sql.DB

const (
	driveName  = "mysql"
	dbUser     = "root"
	dbPass     = "1111"
	dbProtocol = "tcp"
	dbAddress  = "127.0.0.1:3306"
	dbName     = "go_ab"
)

func InitDB() {
	//数据源名
	conStr := dbUser + ":" + dbPass + "@" + dbProtocol + "(" + dbAddress + ")" + "/" + dbName
	dbSQL, err := sql.Open(driveName, conStr)
	if err != nil {
		fmt.Println("mysql open is warn :", err)
		return
	}
	//数据库设置
	dbSQL.SetConnMaxLifetime(60)
	dbSQL.SetConnMaxIdleTime(60)
	dbSQL.SetMaxOpenConns(10)
	dbSQL.SetMaxIdleConns(10)

	//连接测试
	err2 := dbSQL.Ping()
	if err2 != nil {
		fmt.Println("mysql is connect ping failed :", err2)
		return
	}
	DBRead = dbSQL
	fmt.Println("mysql is connect success")
}
