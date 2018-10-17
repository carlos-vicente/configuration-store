package routes

import (
	"github.com/labstack/echo"
)

// Register registers all routes on the base url
func Register(e *echo.Echo, base string) {
	e.GET(base + "/projects", GetProjects)
	e.GET(base + "/projects/:id", GetProject)
	e.GET(base + "/projects/:id/keys", GetProjectKeys)
}
