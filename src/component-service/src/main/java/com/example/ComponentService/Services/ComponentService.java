package com.example.ComponentService.Services;

import com.example.ComponentService.DTOs.ComponentDto;
import com.example.ComponentService.Domain.Component;
import com.example.ComponentService.Exceptions.EntityNotFoundException;
import org.springframework.stereotype.Service;

import java.util.HashMap;

@Service
public class ComponentService {

    private static HashMap<String, Component> componentHashMap;
    static {
        componentHashMap = new HashMap<>();
        componentHashMap.put("fc24d48c-9182-4b50-9b7b-a2c4a0e0a2ee", new Component("fc24d48c-9182-4b50-9b7b-a2c4a0e0a2ee", "E-Engine 2.0"));
        componentHashMap.put("303fefff-d076-424d-933a-c793c2e21fab", new Component("303fefff-d076-424d-933a-c793c2e21fab", "Power Control Unit v16'"));
        componentHashMap.put("cb67fb05-1cee-4d18-bdf6-462a9ee3b06c", new Component("cb67fb05-1cee-4d18-bdf6-462a9ee3b06c", "Spoiler XST"));
        componentHashMap.put("f83c19e7-e5a5-47be-a400-5ab098770009", new Component("f83c19e7-e5a5-47be-a400-5ab098770009", "Battery RS156"));
        componentHashMap.put("84d016f6-d822-48a8-b860-b52cd3c874d8", new Component("84d016f6-d822-48a8-b860-b52cd3c874d8", "E-Engine 3.0"));
        componentHashMap.put("835109f0-6d1d-4282-8298-a0b445eb2d0a", new Component("835109f0-6d1d-4282-8298-a0b445eb2d0a", "Spoiler XST"));
        componentHashMap.put("3d033481-4b99-4f06-82e8-eedeb0ef90e3", new Component("3d033481-4b99-4f06-82e8-eedeb0ef90e3", "Battery ST1234"));
        componentHashMap.put("45349b4b-978b-429a-be40-7e60bba82e62", new Component("45349b4b-978b-429a-be40-7e60bba82e62", "Power Control Unit"));
        componentHashMap.put("f5ed4513-8d8b-401c-aab1-d8164cb58b3f", new Component("f5ed4513-8d8b-401c-aab1-d8164cb58b3f", "DC-DC Converter"));
        componentHashMap.put("c0b6b88d-6192-4929-91ea-a6a3175ea852", new Component("c0b6b88d-6192-4929-91ea-a6a3175ea852", "Transmission AUTO123"));
    }

    public ComponentDto getEngineBySerialNumber(String serialNumber){
        validateComponentExist(serialNumber);
        return entityToDto(componentHashMap.get(serialNumber));
    }

    public void validateComponentExist(String serialNumber){
        if(!componentHashMap.containsKey(serialNumber))
            throw new EntityNotFoundException(String.format("Doesn't exist a component with serial number %s",serialNumber));
    }

    public ComponentDto entityToDto(Component component){
        ComponentDto result = new ComponentDto();
        result.setSerialNumber(component.getSerialNumber());
        result.setType(component.getType());
        result.setDescription(component.getDescription());
        return result;
    }
}
