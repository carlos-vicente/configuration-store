package routes

import (
	".."
	"github.com/labstack/echo"
	"net/http"
)

// GetProjects godoc
// @Summary Gets all projects
// @Description Gets all configured projects from configuration storage
// @Produce  json
// @Success 200 {object} config_store.Project[]
// @Failure 500 {object} echo.HTTPError
// @Router /projects [get]
func GetProjects(c echo.Context) error {
	return c.JSON(http.StatusOK, [] config_store.Project{
		{"project 1"},
		{"project 2"},
	})
}


// GetProject godoc
// @Summary Get the identified project
// @Description Gets the identified project from configuration storage
// @Produce json
// @Success 200 {object} config_store.Project
// @Failure 500 {object} echo.HTTPError
// @Param id path string true "projects identifier" default(A)
// @Router /projects/{id} [get]
func GetProject(c echo.Context) error {
	return c.JSON(http.StatusOK, config_store.Project{
		Name: c.Param("id"),
	})
}
