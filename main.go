package main

import (
	"./config-store/routes"
	"./docs" // docs is generated by Swag CLI, you have to import it.
	"github.com/labstack/echo"
	"github.com/labstack/echo/middleware"
	"github.com/swaggo/echo-swagger"
)

// @title Configuration store
// @version 1.0
// @description This is configuration store server.
// @termsOfService http://swagger.io/terms/

// @contact.name API Support
// @contact.url http://www.swagger.io/support
// @contact.email support@swagger.io

// @license.name Apache 2.0
// @license.url http://www.apache.org/licenses/LICENSE-2.0.html

// @BasePath /api/v1

func main() {

	docs.SwaggerInfo.Host = ""

	e := echo.New()
	e.Use(middleware.Logger())
	routes.Register(e, "/api/v1")
	e.GET("/swagger/*", echoSwagger.WrapHandler)
	e.Logger.Fatal(e.Start(":1323"))
}