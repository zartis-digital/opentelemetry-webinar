package com.example.ComponentService.Domain;

import java.time.Instant;

public class Component {

    private String serialNumber;

    private String type;

    private String description;

    private Instant creationDateTime;


    public Component(String serialNumber, String type){

        if(serialNumber == null || serialNumber.trim().isEmpty())
            throw new IllegalArgumentException("Component serial number was null or empty or a set of blank spaces. Please provide valid serial number.");

        if(serialNumber.startsWith("C"))
            throw new IllegalArgumentException("Component serial numbers starting with C are considered test components and are not allowed.");

        this.serialNumber = serialNumber;
        this.type = type;
        this.description = String.format("Component with serial number '%s' and type '%s'", serialNumber, type);
        this.creationDateTime = Instant.now();
    }


    public String getSerialNumber() {
        return serialNumber;
    }

    public String getType() {
        return type;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public Instant getCreationDateTime() {
        return creationDateTime;
    }

    public void setCreationDateTime(Instant creationDateTime) {
        this.creationDateTime = creationDateTime;
    }

}
