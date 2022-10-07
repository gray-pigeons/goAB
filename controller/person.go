package controller

import (
	"fmt"
	"goAB/pb"
)

// 添加个人信息
func addPersonInfo() {
	uuid := 1234
	name := "john"
	email := "123123@qq.com"
	number := "123-123123131"
	phoneType := pb.Person_HOME

	person := pb.Person{
		Uuid:  int32(uuid),
		Name:  name,
		Email: email,
		Phones: []*pb.Person_PhoneNumber{
			{Number: number, Type: phoneType},
		},
	}

	fmt.Println(&person)

}

// 查询个人信息
func selectPersonInfo() {

}
