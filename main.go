package main

import (
	api_routes "configuration-store/api/routes"
	"configuration-store/docs" // docs is generated by Swag CLI, you have to import it.
	"configuration-store/services"
	web_routes "configuration-store/web/routes"
	"fmt"
	"github.com/foolin/echo-template"
	_ "github.com/jinzhu/gorm/dialects/mssql"
	"github.com/labstack/echo"
	"github.com/labstack/echo/middleware"
	"github.com/paked/configure"
)


var (
	conf = configure.New()
	port = conf.Int("port", 8080, "The port to listen for requests")
	connectionString = conf.String("connection-string", "sqlserver://sa:yourStrong(!)Password@192.168.137.7:1433?database=cenas", "Configuration store connection string")
)

func init() {
	conf.Use(configure.NewEnvironment())
	conf.Use(configure.NewFlag())
}

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
	//e.Use(middleware.Secure()) // breaks swagger ui integration -> find out why

	templateConfig := echotemplate.TemplateConfig{
		Delims:			echotemplate.DefaultConfig.Delims,
		DisableCache:	echotemplate.DefaultConfig.DisableCache,
		Extension:		echotemplate.DefaultConfig.Extension,
		Funcs:			echotemplate.DefaultConfig.Funcs,
		Partials:		echotemplate.DefaultConfig.Partials,
		Master:			"layouts/master",
		Root:			"web/views",
	}
	e.Renderer = echotemplate.New(templateConfig)

	conf.Parse()

	fmt.Printf("Connecting to %s", *connectionString)

	/*db, err := gorm.Open("mssql", *connectionString) //dereference pointer
	if err != nil {
		e.Logger.Fatal("failed to connect database")
	}
	defer db.Close()

	db.AutoMigrate(&api.Project{})*/

	configKeysService := services.ConfigurationKeyService{
		DbConnection: *connectionString,
	}

	api_routes.Register(e, "/api/v1", &configKeysService)
	web_routes.Register(e, "")
	web_routes.RegisterSwaggerUi(e, "")
	e.Static("/static", "web/dist")

	e.Logger.Fatal(e.Start(fmt.Sprintf(":%d", *port))) //dereference pointer
}
