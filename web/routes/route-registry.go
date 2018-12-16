package routes

import (
	"github.com/labstack/echo"
	"github.com/swaggo/swag"
	"net/http"
	"time"
)

type Nav struct {
	Rel string `json:"rel"`
	Link string `json:"link"`
} 

type ConfigKeyListItem struct {
	Key string			`json:"key"`
	Type string			`json:"type"`
	CreatedAt time.Time	`json:"createdAt"`
	Links []Nav			`json:"links"`
}

type ConfigKeyList struct {
	Title string
	ConfigKeys []ConfigKeyListItem `json:"configKeys"`
}

// Register registers all routes on the base url
func Register(e *echo.Echo, base string) {
	e.GET(base + "/", func(context echo.Context) error {

		data := &ConfigKeyList{
			Title: "Configuration keys",
			ConfigKeys: []ConfigKeyListItem{
				{
					Key:"key",
					Type:"String",
					CreatedAt: time.Now(),
					Links: []Nav{
						{"self", "/key/key"},
					},
				},
			},
		}

		return context.Render(
			http.StatusOK,
			"index",
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