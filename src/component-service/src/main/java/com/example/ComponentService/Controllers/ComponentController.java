package com.example.ComponentService.Controllers;

import com.example.ComponentService.DTOs.ComponentDto;
import com.example.ComponentService.Services.ComponentService;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import java.util.ArrayList;
import java.util.List;

@RestController
@RequestMapping(produces = {MediaType.APPLICATION_JSON_VALUE})
public class ComponentController {

    private final ComponentService _componentService;

    public ComponentController(ComponentService componentService){
        _componentService = componentService;
    }

    @GetMapping(value = "/Components")
    ResponseEntity<List<ComponentDto>> getComponents()
    {
        List<ComponentDto> result = new ArrayList<>(); // Simulate business call here
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping("/Components/{serialNumber}")
    ResponseEntity<ComponentDto> getComponent(@PathVariable String serialNumber)
    {
        ComponentDto result = _componentService.getEngineBySerialNumber(serialNumber);
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @PostMapping("/Components")
    ResponseEntity<ComponentDto> createComponent(@RequestBody ComponentDto newComponentDto)
    {
        // Simulate business call here
        return new ResponseEntity<>(newComponentDto, HttpStatus.CREATED);
    }

    @PutMapping("/Components/{serialNumber}")
    ResponseEntity<ComponentDto> modifyComponent(@PathVariable String serialNumber, @RequestBody ComponentDto modifiedComponentDto)
    {
        // Simulate business call here
        return new ResponseEntity<>(modifiedComponentDto, HttpStatus.OK);
    }

    @DeleteMapping("/Components/{serialNumber}")
    ResponseEntity<ComponentDto> deleteComponent(@PathVariable String serialNumber)
    {
        // Simulate business call here
        return new ResponseEntity<>(HttpStatus.OK);
    }
}
