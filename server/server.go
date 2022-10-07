package server

import (
	"io"
	"log"
	"net"
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
		handleConn(conn)
	}
}

func handleConn(conn net.Conn) {
	defer conn.Close()
	for {
		_, err1 := io.WriteString(conn, time.Now().Format("15:04:05\n"))
		if err1 != nil {
			return
		}
		time.Sleep(1 * time.Second)
	}
}
