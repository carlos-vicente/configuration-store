package routes

import (
	"configuration-store/services"
	"github.com/labstack/echo"
	"net/http"
)

// GetProjectKeys godoc
// @Summary Gets all project's keys
// @Description Gets all configured project's keys from configuration storage
// @Produce json
// @Success 200 {object} api.Key[]
// @Failure 500 {object} echo.HTTPError
// @Router /keys [get]
func GetProjectKeys(service services.ConfigurationKeys) func(c echo.Context) error {
	return func(c echo.Context) error{
		keys := service.GetKeys()
		return c.JSON(http.StatusOK, keys)
	}
}


// GetProjectKeys godoc
// @Summary Gets a project's specific key
// @Description Gets a specific configured project's key from configuration storage
// @Produce json
// @Success 200 {object} api.Key
// @Failure 500 {object} echo.HTTPError
// @Param id path string true "key identifier" default(A)
// @Router /keys/{id} [get]
func GetProjectKey(service services.ConfigurationKeys) func(c echo.Context) error {
	return func(c echo.Context) error{
		keys := service.GetKey(c.Param("id"))
		return c.JSON(http.StatusOK, keys)
	}
}
