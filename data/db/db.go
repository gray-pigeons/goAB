package data

import (
	"database/sql"
	"fmt"
)

//-------连接数据库----------------

var dbRead *sql.DB

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
	dbRead = dbSQL
	fmt.Println("mysql is connect success")
}

type User struct {
	Id       int
	Username string
	Password string
}

var (
	id_          int
	name_, pass_ string
)

// 查看用户和密码是否正确
func IsExist(name string) (User, error) {
	row, err := dbRead.Query("select * from user where name=?;", name)
	if err != nil {
		fmt.Println("查询用户出错:", err)
		return User{}, err
	}

	for row.Next() {
		err2 := row.Scan(&id_, &name_, &pass_)
		if err2 != nil {
			fmt.Println("解析用户名和密码发生错误:", err2)
			return User{}, err2
		}

		fmt.Println("查找结果:", id_, name_, pass_)
	}
	defer row.Close()
	return User{id_, name_, pass_}, nil
}
