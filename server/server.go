package server

import (
	"fmt"
	"io"
	"log"
	"net"
	"os"
	"time"
)

// 初始化tcp服务
func InitServer() {
	listener, err := net.Listen("tcp", "localhost:8081")
	if err != nil {
		log.Fatal(err)
		panic(err)
	}

	for {
		conn, conErr := listener.Accept()
		if conErr != nil {
			log.Println(conErr)
			continue
		}
		go handleConn(os.Stdout, conn)
	}
}

func handleConn(dst io.Writer, conn net.Conn) {
	fmt.Println("收到新连接:", conn.RemoteAddr().String())
	go jump(&conn)
	defer conn.Close()
	if _, err := io.Copy(dst, conn); err != nil {
		log.Fatal(err)
	}

}

func jump(conn *net.Conn) {
	for {
		str1 := "服务器心跳:" + time.Now().Format("15:04:05\n")
		_, err1 := io.WriteString(*conn, str1)
		if err1 != nil {
			return
		}
		time.Sleep(12 * time.Second)
	}
}
