package routes

import (
	"configuration-store/services"
	"github.com/labstack/echo/v4"
)

// Register registers all routes on the base url
func Register(e *echo.Echo, base string, service services.ConfigurationKeys) {
	/*e.GET(base+"/projects", GetProjects)
	e.GET(base+"/projects/:id", GetProject)
	e.GET(base+"/projects/:id/keys", GetProjectKeys)*/

	e.GET(base + "/keys", GetProjectKeys(service))
	e.GET(base + "/keys/:id", GetProjectKey(service))
}
