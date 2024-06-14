import React, { useEffect, useRef, useState } from 'react';
import { ConversationGet, Message } from '../../Models/Message';
import { handleError } from '../../Helpers/ErrorHandler';
import { ConversationsGetAPI, DeleteConversationAPI, DeleteMessagesAPI } from '../../Service/MessageService';
import * as Yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { useForm } from 'react-hook-form';
import { MessagePostAPI, GetMessagesAPI } from '../../Service/MessageService';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import DeleteIcon from '@mui/icons-material/Delete';
import Modal from '../../Components/Modal/Modal';
import Spinner from '../../Components/Spinners/Spinner';

type Props = {}

type MessageInputs = {
    content: string;
}

const validation = Yup.object().shape({
    content: Yup.string()
        .required('')
        .min(1)
        .max(300)
        .transform((value) => value.replace(/\s+/g, ' ').trim()),
});

const MessagesPage: React.FC<Props> = () => {
    const [conversationValues, setConversationValues] = useState<ConversationGet[] | null>([]);
    const [clickedConversation, setClickedConversation] = useState<ConversationGet | null>(null);
    const [messages, setMessages] = useState<Message[] | null>(null);
    const [showMessages, setShowMessages] = useState<boolean>(false);
    const [showModal, setShowModal] = useState<boolean>(false);
    const [deleteAction, setDeleteAction] = useState<() => void>(() => { });
    const [modalTitle, setModalTitle] = useState<string>('');
    const [modalDescription, setModalDescription] = useState<string>('');
    const [isButtonDisabled, setIsButtonDisabled] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>();

    const handleGetConversations = async () => {
        
        try {
            const response = await ConversationsGetAPI();
            if (response && (response.data.length >= 1)) { setConversationValues(response.data); }
            else { setConversationValues(null); }
        } catch (error) {
            handleError(error); setConversationValues(null);
        }
    }

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            await handleGetConversations();
            setLoading(false); 
        };
        fetchData();
    }, []);

    const {
        handleSubmit,
        register,
        formState: { errors },
        reset
    } = useForm<MessageInputs>({ resolver: yupResolver(validation) });

    const handleMessageSend = async (data: MessageInputs) => {
        try {
            if (clickedConversation) {
                const response = await MessagePostAPI(clickedConversation.conversationId, data.content);
                handleGetMessages();
                formRef.current?.reset();
            }
        } catch (error) {
            handleError(error);
        }
        setIsButtonDisabled(true);
        setTimeout(() => setIsButtonDisabled(false), 1000);
        scrollToBottom();
    };

    const handleGetMessages = async () => {
        try {
            if (clickedConversation?.conversationId) {
                const response = await GetMessagesAPI(clickedConversation.conversationId);
                if (response && response.data) {
                    setMessages(response.data);
                }
            }
        } catch (error) {
            handleError(error);
        }
    }

    useEffect(() => {
        const fetchDataAndScrollToBottom = async () => {
            if (clickedConversation) {
                await handleGetMessages();
                formRef.current?.reset();
                scrollToBottom();
            }
        };

        fetchDataAndScrollToBottom();
    }, [clickedConversation]);

    const handleClick = (conversation: ConversationGet) => {
        setClickedConversation(conversation);
        setShowMessages(true);
    };

    const handleBackToConversations = () => {
        setClickedConversation(null);
        setShowMessages(false);
    };

    const handleDeleteConversation = async (conversationId: number) => {
        try {
            if (conversationId) {
                await DeleteConversationAPI(conversationId);
            }
        } catch (error) {
            handleError(error);
        } finally {
            setShowModal(false);
            setDeleteAction(() => { });
            handleGetConversations();
        }
    }

    const handleDeleteMessages = async () => {
        try {
            if (clickedConversation?.conversationId) {
                const response = await DeleteMessagesAPI(clickedConversation.conversationId);
                if (response && response.data) {
                    setMessages(response.data);
                }
            }
        } catch (error) {
            handleError(error);
        } finally {
            setShowModal(false);
            setDeleteAction(() => { });
        }
    }

    const messagesEndRef = useRef<HTMLDivElement>(null);
    const formRef = useRef<HTMLFormElement>(null);

    const scrollToBottom = () => {
        setTimeout(() => {
            if (messagesEndRef.current != null) {
                messagesEndRef.current.scrollIntoView({ behavior: 'smooth', block: 'end' });
            }
        }, 100);
    };

    useEffect(() => {
        const interval = setInterval(() => {
            handleGetConversations();
            if (clickedConversation) {
                handleGetMessages();
            }
        }, 5000);
        return () => clearInterval(interval);
    }, [clickedConversation]);

    return (
        <div className='flex  mx-3 justify-center relative'>
            <div className="flex md:flex-row w-full m-2 md:w-3/4 lg:2/3">
                {/* Conversations */}
                <div className={`overflow-y-auto w-full  md:w-5/12 lg:1/3 p-3 border border-black rounded-lg md:rounded-s-lg ${showMessages ? 'hidden md:block' : ''} min-h-[calc(70vh)] max-h-[calc(70vh)]`}>
                    <h1 className='text-center font-light w-full border bg-gray-100 border-gray-400 rounded-xl py-2 text-md lg:text-lg xl:text-2xl m-0'>Messages</h1>
                    {loading ? < Spinner /> : (
                        <>
                            {conversationValues ? (conversationValues.map((conversation) => (
                                <div key={conversation.conversationId} className="relative flex flex-row mt-4 bg-slate-50 items-center text-center rounded-md shadow-md opacity-80 hover:bg-slate-200 ">
                                    <img className='xxs:w-6 xs:w-10 md:w-12 lg:12 items-center rounded-full'
                                    src={conversation.pictureLink === "http://localhost:5279/resources/empty" ? "/img/profile.png" : conversation.pictureLink}
                                    onClick={() => handleClick(conversation)} alt="" />
                                    <p className='lg:hidden px-1 items-center font-serif xxs:text-[12px] xs:text-[13px] text-darkBlue pr-2' onClick={() => handleClick(conversation)}>
                                        {conversation.userFullName.slice(0, 15)}{conversation.userFullName.length > 15 ? '...' : ''}
                                    </p>
                                    <p className='hidden lg:block px-1 items-center font-serif text:md text-darkBlue pr-2' title={conversation.userFullName} onClick={() => handleClick(conversation)}>
                                        {conversation.userFullName.slice(0, 20)}{conversation.userFullName.length > 22 ? '...' : ''}
                                    </p>
        
                                    {conversation.numberOfUnreadMessages > 0 && (
                                        <>
                                            <span className="absolute right-12 md:right-14 justify-center items-center w-6 h-6 bg-red-800 text-white rounded-full animate-ping"></span>
                                            <span className="absolute right-12 md:right-14 justify-center items-center w-6 h-6 bg-red-600 text-white rounded-full">
                                                {conversation.numberOfUnreadMessages}
                                            </span>
                                        </>
                                    )}
                                    <button onClick={() => {
                                        setShowModal(true);
                                        setDeleteAction(() => () => handleDeleteConversation(conversation.conversationId));
                                        setModalTitle('Delete Conversation');
                                        setModalDescription('Are you sure you want to delete this conversation?');
                                    }}
                                        className="absolute right-2 p-2 bg-gray-700 text-white rounded-lg hover:bg-gray-500"
                                        title="Delete Conversation"
                                    >
                                        <DeleteIcon color='inherit' fontSize={'small'} />
                                    </button>
                                </div>
                                
                            ))) : (<h1 className='text-center mt-2'>You don't have any conversation</h1>) } </>
                    )}
                    
                </div>

                {/* Messages */}
                {clickedConversation && (
                    <div key={clickedConversation.conversationId}
                        className={`flex flex-col relative w-full md:w-7/12 lg:2/3 p-3 border border-black rounded-lg md:roundedé-e-lg ${showMessages ? 'block' : 'hidden md:flex'} min-h-[calc(70vh)] max-h-[calc(70vh)]`}>
                        <div className="top-0 bg-gray-100 p-4 border border-gray-300 rounded-xl mb-5 flex justify-between items-center">
                            <button onClick={handleBackToConversations} className="md:hidden p-2 bg-gray-600 text-white rounded-lg">
                                <ArrowBackIosIcon color='inherit' fontSize={'small'} /></button>
                            <h1 className="text-center text-gray-700 font-serif text-lg xl:text-2xl m-0">
                                {clickedConversation.userFullName}
                            </h1>
                            <button
                                onClick={() => {
                                    setShowModal(true);
                                    setDeleteAction(() => handleDeleteMessages);
                                    setModalTitle('Delete Messages');
                                    setModalDescription('Are you sure you want to delete all messages in this conversation?');
                                }}
                                className="p-2 bg-gray-600 text-white rounded-lg hover:bg-gray-800"
                                title="Delete Messages"
                            >
                                <DeleteIcon color='inherit' fontSize={'small'} />
                            </button>
                        </div>
                        <div className='overflow-y-auto h-full pb-6 '>
                            {messages && messages.map((message, index) => (
                                <div key={index} className={`flex py-1 ${message.senderId === clickedConversation.userId ? 'justify-start' : 'justify-end'}`}>
                                    {message.senderId == clickedConversation.userId ? (<img className='w-10 h-10 rounded-full' src={clickedConversation.pictureLink === "http://localhost:5279/resources/empty" ? "/img/profile.png" : clickedConversation.pictureLink} alt="" />
                                    )
                                        :
                                        (<></>)}
                                    <div className={`max-w-48 sm:max-w-64 lg:max-w-72 text-sm xl:text-sm place-content-center mx-px px-2 py-px shadow-md rounded-lg ${message.senderId === clickedConversation.userId ? 'text-white bg-darkBlue' : 'bg-gray-300 text-black'}`}>
                                        <p className="break-words">{message.content}</p>
                                        <span className={`flex place-items-end text-[9px] ${message.senderId === clickedConversation.userId ? 'text-gray-300' : 'text-gray-600'}`}>
                                            {new Date(message.sentDate).toLocaleString()}
                                        </span>
                                    </div>
                                </div>
                            ))}
                            <div ref={messagesEndRef} />
                        </div>
                        <div className="pt-4">
                            <form ref={formRef} onSubmit={handleSubmit(handleMessageSend)} className="flex flex-row justify-between items-center gap-2">
                                <textarea {...register('content')} placeholder='Type message here...' rows={1} maxLength={300}
                                    className={`p-3 border rounded-md shadow-md w-full resize-none  focus:outline-none focus:ring-2 focus:ring-darkBlue`} />
                                <button type='submit'
                                disabled={isButtonDisabled}
                                className="p-2 border rounded-lg shadow-md bg-darkBlue text-white hover:animate-pulse">Send</button>
                            </form>

                        </div>
                    </div>
                )}
            </div>
            {showModal && (
                <Modal
                    title={modalTitle}
                    description={modalDescription}
                    onClose={() => setShowModal(false)}
                    onConfirm={deleteAction}
                />
            )}
        </div>
    );
}

export default MessagesPage;
