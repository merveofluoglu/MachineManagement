import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import ModalComponent from '../ModalComponent';

function WebSocketComponent() {

    const [messages, setMessages] = useState([]);
    const [open, setOpen] = useState(false);
    const [currentMsg, setCurrentMsg] = useState('');

    const handleClose = () => setOpen(false);

    useEffect(() => {
        const _connection = new signalR.HubConnectionBuilder()
            .withUrl('http://localhost:7040/messagehub', {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();

        _connection.on('ReceiveMessage', (message) => {
            console.log("message: ", message)
            setMessages((prevMessages) => [...prevMessages, message]);
            setCurrentMsg(message);
            setOpen(true);
        });

        _connection.start()
            .then(() => {
                console.log('SignalR connected');
            })
            .catch(err => {
                console.error('Connection error:', err);
            });

        return () => {
            _connection.stop();
        };

    }, []);

    return (
        <div>
            <ModalComponent open={open} handleClose={handleClose} message={currentMsg} />
        </div>
    );
}

export default WebSocketComponent;