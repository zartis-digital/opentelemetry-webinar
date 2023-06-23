package com.example.ComponentService.Exceptions;

public class EntityNotFoundException extends RuntimeException{

    public EntityNotFoundException(String Message){
        super(Message);
    }
}
