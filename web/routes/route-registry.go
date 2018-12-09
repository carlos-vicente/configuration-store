package routes

import (
	"github.com/labstack/echo"
	"github.com/swaggo/swag"
	"net/http"
)

type Project struct {
	Name string `json:"name"`
}

// Register registers all routes on the base url
func Register(e *echo.Echo, base string) {
	e.GET(base + "/", func(context echo.Context) error {
		data := &Project{
			"some project",
		}

		return context.Render(
			http.StatusOK,
			"index.html",
			data)
	})
}


func RegisterSwaggerUi(e *echo.Echo, base string){
	e.GET(base + "/swagger", func(context echo.Context) error {
		model := struct {
			Docs string
		}{"/swagger/docs"}
		return context.Render(
			http.StatusOK,
			"swagger-ui.html",
			model)
	})

	e.GET(base + "/swagger/docs", func(context echo.Context) error {
		doc, _ := swag.ReadDoc()

		context.Response().Header().Set(echo.HeaderContentType, echo.MIMEApplicationJSONCharsetUTF8)
		context.Response().WriteHeader(http.StatusOK)

		context.Response().Write([]byte(doc))
		return nil
	})
}