package routes

import (
	".."
	"github.com/labstack/echo"
	"net/http"
)

// GetProjectKeys godoc
// @Summary Gets all project's keys
// @Description Gets all configured project's keys from configuration storage
// @Produce json
// @Success 200 {object} config_store.Key[]
// @Failure 500 {object} echo.HTTPError
// @Param id path string true "projects identifier" default(A)
// @Router /projects/{id}/keys [get]
func GetProjectKeys(c echo.Context) error {
	return c.JSON(http.StatusOK, []config_store.Key{
		{"key 1"},
		{"key 2"},
	})
}
