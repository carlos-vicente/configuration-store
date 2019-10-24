package routes

import (
	"configuration-store/api"
	"github.com/labstack/echo/v4"
	"net/http"
)

// GetProjects godoc
// @Summary Gets all projects
// @Description Gets all configured projects from configuration storage
// @Produce  json
// @Success 200 {object} api.Project[]
// @Router /projects [get]
func GetProjects(c echo.Context) error {
	return c.JSON(http.StatusOK, []api.Project{
		{Name: "project 1"},
		{Name: "project 2"},
	})
}

// GetProject godoc
// @Summary Get the identified project
// @Description Gets the identified project from configuration storage
// @Produce json
// @Success 200 {object} api.Project
// @Param id path string true "projects identifier" default(A)
// @Router /projects/{id} [get]
func GetProject(c echo.Context) error {
	return c.JSON(http.StatusOK, api.Project{
		Name: c.Param("id"),
	})
}
