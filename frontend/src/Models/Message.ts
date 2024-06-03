export type ConversationGet = {
    conversationId: number;
    userId: string;
    userFullName: string;
    lastReadDate: Date;
    numberOfUnreadMessages: number;
    pictureLink: string;
}
export type MessagePost = {
    conversationId: number;
    content: string;
}

export type Message = {
    messageId: number;
    senderId: string;
    content: string;
    sentDate: Date;
}
