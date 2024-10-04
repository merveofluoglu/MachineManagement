import React from 'react';
import { Modal, Box, Typography, Button } from '@mui/material';

function ModalComponent({ open, handleClose, message }) {

    const style = {
        position: 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        widt: 400,
        bgcolor: 'background.paper',
        boxShadow: 24,
        p: 4,
        overflow: scroll
    };

    return (
        <Modal
            open={open}
            onClose={handleClose}
            aria-labelledby='modal-modal-title'
            aria-describedby='modal-modal-description'
        >
            <Box sx={style}>
                <Typography id="modal-modal-title" variant="h6" component="h2">
                    New Message
                </Typography>
                <Typography id="modal-modal-description" sx={{ mt: 2 }}>
                    {message}
                </Typography>
                <Button onClick={handleClose}>
                    Close
                </Button>
            </Box>
        </Modal >
    );
}

export default ModalComponent;