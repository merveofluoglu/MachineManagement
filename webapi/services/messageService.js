import React, { useState } from 'react';
import axios from 'axios';

const url = 'https://localhost:7206/api/Messages';

const messagesService = {
    getMessagesByClientId: async (id) => {
        try {
            const response = await axios.get(`${url}/ClientId/${id}`);
            console.log(response.data);
            if (response.data && response.data.length > 0) {
                const columns = Object.keys(response.data[0]).map(key => ({
                    id: key,
                    label: key
                        .replace(/_/g, ' ')                 
                        .replace(/([a-z])([A-Z])/g, '$1 $2') 
                        .replace(/^./, str => str.toUpperCase())
                }));
                return { messages: response.data, columns };
            }

            return { messages: [], columns: [] };
        } catch (error) {
            console.error("Error occurred while fetching messages:", error);
            throw error;
        }
    },

    getAll: async () => {
        try {
            const response = await axios.get(url);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while fetching messages:", error);
            throw error;
        }
    },

    getMessageById: async (id) => {
        try {
            const response = await axios.get(`${url}/MessageId/${id}`);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while fetching the message:", error);
            throw error;
        }
    },

    addMessage: async (message) => {
        try {
            const response = await axios.post(url, message);
            console.log("Message ades to the database:", response.data);
            return response.data;
        } catch (error) {
            console.error("Error accured while adding message to database:", error);
            throw error;
        }
    },

    deleteMessage: async (id) => {
        try {
            const response = await axios.delete(`${url}/MessageId/${id}`);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured while deleting message:", error);
            throw error;
        }
    },

    readMessage: async (id) => {
        try {
            const response = await axios.post(`${url}/id/${id}`);
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error("Error occured:", error);
            throw error;
        }
    }
};

export default messagesService;