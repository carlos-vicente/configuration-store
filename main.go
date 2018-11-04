package main

import (
	api_routes "configuration-store/api/routes"
	"configuration-store/docs" // docs is generated by Swag CLI, you have to import it.
	web_routes "configuration-store/web/routes"
	"fmt"
	"github.com/foolin/echo-template"
	"github.com/labstack/echo"
	"github.com/labstack/echo/middleware"
	"github.com/swaggo/echo-swagger"
	"os"
	"strconv"
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

	e.Use(middleware.RequestID())
	e.Use(middleware.Logger())
	e.Use(middleware.Recover())
	e.Use(middleware.CSRF())
	e.Use(middleware.Secure())


	templateConfig := echotemplate.TemplateConfig{
		Delims:			echotemplate.DefaultConfig.Delims,
		DisableCache:	echotemplate.DefaultConfig.DisableCache,
		Extension:		echotemplate.DefaultConfig.Extension,
		Funcs:			echotemplate.DefaultConfig.Funcs,
		Partials:		echotemplate.DefaultConfig.Partials,
		Master:			"web/layouts/master",
		Root:			"web/views",
	}
	e.Renderer = echotemplate.New(templateConfig)

	api_routes.Register(e, "/api/v1")
	web_routes.Register(e, "/")

	e.GET("/swagger/*", echoSwagger.WrapHandler)

	var port int
	args := os.Args[1:]
	if len(args) > 0 {
		p, error := strconv.Atoi(args[0])
		if error != nil{
			e.Logger.Fatal(error)
		}
		port = p
	} else {
		port = 8080
	}

	e.Logger.Fatal(e.Start(fmt.Sprintf(":%d", port)))
}
