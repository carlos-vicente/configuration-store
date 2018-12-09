package services

import(
	"configuration-store/api"
)

type ConfigurationKeys interface{
	GetKeys() []api.Key
	GetKey(id string) api.Key
}

type ConfigurationKeyService struct{
	DbConnection string
}


func (service *ConfigurationKeyService) GetKeys() []api.Key{
	return []api.Key{
		{ Id: "1", Name:"key 1"},
		{ Id: "2", Name:"key 2"},
	}
}

func (service *ConfigurationKeyService) GetKey(id string) api.Key{
	return api.Key{Id: id, Name:"some name"}
}