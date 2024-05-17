import React, { useState } from 'react';
import axios from 'axios';

const url = 'https://localhost:7206/api/Machines';

const machineService = {
    getAllMachines: async () => {
        try {
            const response = await axios.get(url);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occurred while fetching machines:", error);
            throw error;
        }
    },

    getMachineById: async (machineId) => {
        try {
            const response = await axios.get(`${url}/MachineId/${machineId}`);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while fetching machine:", error);
            throw error;
        }
    },

    getMachineByName: async (machineName) => {
        try {
            const response = await axios.get(`${url}/MachineName/${machineName}`);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while fetching machine:", error);
            throw error;
        }
    },

    addMachine: async (machine) => {
        try {
            const response = await axios.post(url, machine);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while adding machine to database:", error);
            throw error;
        }
    },

    updateMachine: async (machine) => {
        try {
            const response = await axios.put(url, machine);
            console.log(reponse.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while updating machine:", error);
            throw error;
        }
    },

    deleteMachine: async (id) => {
        try {
            const response = await axios.delete(`${url}/${id}`);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while deleting machine:", error)
            throw error;
        }
    },

    getMessageCountOfMachine: async (id) => {
        try {
            const response = await axios.get(`${url}/ClientId/${id}`);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while fetching the message count:", error);
            throw error;
        }
    }
};

export default machineService;