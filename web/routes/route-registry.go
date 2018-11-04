package routes

import (
	"github.com/labstack/echo"
	"net/http"
)

type Project struct {
	Name string `json:"name"`
}

// Register registers all routes on the base url
func Register(e *echo.Echo, base string) {
	e.GET(base, func(context echo.Context) error {
		data := &Project{
			"some project",
		}

		return context.Render(
			http.StatusOK,
			"index.html",
			data)
	})
}