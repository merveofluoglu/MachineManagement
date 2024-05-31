import React, { useState } from 'react';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Modal from '@mui/material/Modal';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import messageService from '../services/messageService';
import CustomDialog from './CustomDialog';

const MessageComponent = ({ isOpen, handleClose, messages, columns, machineId }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [dialog, setDialog] = useState(false);
    const [message, setMessage] = useState('');

    const handleReadFullMessage = async (messageId) => {
        await getMessageDetails(messageId);
    };

    const handleDeleteMessage = async (messageId) => {
        await deleteMessage(messageId);
    };

    const handleDialogOpen = () => {
        setDialog(true);
    }

    const handleDialogClose = () => {
        setDialog(false);
    }

    const getMessageDetails = async (id) => {
        try {
            const response = await messageService.getMessageById(id);
            console.log(response);
            setMessage(response);
            handleDialogOpen();
        } catch (error) {
            console.error('Error reading the full message:', error);
        }
    }

    const deleteMessage = async (id) => {
        try {
            const response = messageService.deleteMessage(id);
            console.log(response);
        } catch (error) {
            console.error('Error deleting the message:', error);
        }
    }

    const filteredMessages = messages.filter(message => // For the search implementation. 
        Object.values(message).some(value =>
            String(value).toLowerCase().includes(searchTerm.toLowerCase())
        )
    );

    const style = {
        position: 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: '80%',
        bgcolor: 'background.paper',
        boxShadow: 24,
        p: 4,
    };

    return (
        <Modal open={isOpen} onClose={handleClose}>
            <Box sx={style}>
                <Typography variant="h6" component="h2" gutterBottom>
                    Messages From Machine {machineId}
                </Typography>
                <TextField
                    fullWidth
                    variant="outlined"
                    label="Search Messages"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    sx={{ mb: 2 }}
                />
                {messages.length === 0 ? (
                    <Typography>No messages available.</Typography>
                ) : (
                        <TableContainer component={Paper}>
                            <Table>
                                <TableHead>
                                    <TableRow> {/*//Table column names with actions*/}
                                        {columns.map((column) => (
                                            <TableCell key={column.id}>{column.label}</TableCell>
                                        ))}
                                        <TableCell align="right">Actions</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {filteredMessages.map((message) => (
                                        <TableRow key={message.id}>
                                            {columns.map((column) => (
                                                <TableCell key={column.id}>
                                                    {message[column.id] !== null && message[column.id] !== undefined //Ckecking if message contains null or undefined columns
                                                        ? String(message[column.id])
                                                        : ''}
                                                </TableCell>
                                            ))}
                                            <TableCell align="right">
                                                <Button
                                                    onClick={() => handleReadFullMessage(message.id)}
                                                    color="primary"
                                                >
                                                    Read
                                                </Button>
                                                <Button
                                                    onClick={() => handleDeleteMessage(message.id)}
                                                    color="secondary"
                                                >
                                                    Delete
                                                </Button>
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                )}
                <Box sx={{ mt: 2 }}>
                    <Button onClick={handleClose} color="primary" variant="contained">
                        Close
                    </Button>
                </Box>
                <CustomDialog
                    dialog={dialog}
                    handleCloseDialog={handleDialogClose}
                    message={message}
                >
                </CustomDialog>
            </Box>            
        </Modal>
    );
};

export default MessageComponent;
