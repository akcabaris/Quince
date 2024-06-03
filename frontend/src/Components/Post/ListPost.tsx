import React, { useEffect, useState } from 'react';
import { PostGet } from '../../Models/Post';
import SquarishPost from '../Post/SquarishPost';
import { useAuth } from '../../Context/useAuth';


interface Props {
    postValues: PostGet[];
    totalPost: number;
}

const ListPost = ({ postValues, totalPost }: Props) => {
    const [openIndex, setOpenIndex] = useState<number | null>(null);
    const {user} = useAuth();

    const handleToggle = (index: number) => {
        setOpenIndex(openIndex === index ? null : index);
    };

    return (
        <section id="posts" className="">
                {
                postValues.length > 0 ? (
                        <div className='flex flex-col overflow-hidden bg-white items-center justify-center'>
                            <h1 className='text-2xl font-mono text-gray-600 pb-6 mt-6 max-w-90 text-center'>
                                {totalPost} Results Found For Your Search
                            </h1>
                            <div className='item-center lg:grid w-full lg:grid-cols-2'>
                            {postValues.map((postVal, index) => (
                                <SquarishPost
                                    key={postVal.postId}
                                    postValue={postVal}
                                    isOpen={index === openIndex}
                                    onToggle={() => handleToggle(index)}
                                />
                                
                            ))}
                            </div>
                        </div>
                    
                ) : (
                    <div className="p-8 text-center text-gray-500">
                        No results found for your search.
                    </div>
                )
                }
        </section>
    );

};

export default ListPost;
