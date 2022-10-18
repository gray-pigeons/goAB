package server

import (
	"bufio"
	"fmt"
	"net"
	"os"
	"strings"
)

// 初始化tcp服务
func InitTcpServer() {

	//获取本地ip地址
	host := ":8081"
	tcpAddr, tcpAddrErr := net.ResolveTCPAddr("tcp4", host)
	//监听器
	var listener net.Listener

	if tcpAddrErr != nil {
		fmt.Println("tcpAddr() failed:", tcpAddrErr)
		inputReader := bufio.NewReader(os.Stdin)
		fmt.Printf("请手动输入ip:")
		input, _ := inputReader.ReadString('\n')
		host = strings.Trim(input, "\r\n")
		host += ":8081"
		listen, err := net.Listen("tcp", host)
		listener = listen
		if err != nil {
			fmt.Println("listen() failed,err:", err)
			return
		}
	} else {
		fmt.Println("获取到的ip是:", tcpAddr)
		listen, err := net.ListenTCP("tcp", tcpAddr)
		listener = listen
		if err != nil {
			fmt.Println("listen() failed,err:", err)
			return
		}
	}

	//建立连接池，用于广播消息
	conPool := make(map[string]net.Conn)

	//消息通道
	messageChan := make(chan string, 10)

	//广播消息
	go BroadMessage(&conPool, messageChan)

	//等待与客户端建立连接
	for {
		conn, conErr := listener.Accept()
		if conErr != nil {
			fmt.Println("Accept() failed:", conErr)
			continue
		}

		//把每个客户端连接入池
		conPool[conn.RemoteAddr().String()] = conn
		//处理收到的每个客户端消息
		go handleClient(conn, &conPool, messageChan)

	}

}

// 广播消息
func BroadMessage(conPool *map[string]net.Conn, message chan string) {
	for {
		//不断从通道中读取消息
		msg := <-message

		//向所有人广播消息
		for key, conn := range *conPool {
			fmt.Println("connection is connected from:", key)
			_, err := conn.Write([]byte(msg))
			if err != nil {
				fmt.Println("broad message to ", key, " failed:", err)
				delete(*conPool, key)
			}
		}
	}
}

// 处理收到的每个客户端消息
func handleClient(conn net.Conn, conPool *map[string]net.Conn, message chan string) {
	fmt.Println("收到新连接:", conn.RemoteAddr().String())

	for {
		reader := bufio.NewReader(conn)
		var buf [128]byte
		//读取数据
		n, readErr := reader.Read(buf[:])
		if readErr != nil {
			fmt.Println("read from client failed ,err:", readErr)
			delete(*conPool, conn.RemoteAddr().String())
			conn.Close()
			break
		}

		//把消息写到通道中
		recvStr := string(buf[:n])
		message <- recvStr
		fmt.Println("收到客户端 ", conn.RemoteAddr().String(), "发来的数据:", recvStr)

	}
}

// 心跳
// func jump(conn *net.Conn) {
// 	for {
// 		str1 := "服务器心跳:" + time.Now().Format("15:04:05\n")
// 		_, err1 := io.WriteString(*conn, str1)
// 		if err1 != nil {
// 			return
// 		}
// 		time.Sleep(12 * time.Second)
// 	}
// }
