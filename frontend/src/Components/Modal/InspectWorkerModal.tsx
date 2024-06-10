import React, { useEffect, useState } from 'react'
import { WorkerGET } from '../../Models/Worker';
import { WorkerGetAPI } from '../../Service/WorkerService';

type Props = {
  workerId: string;
}

const InspectUserModal = ({workerId}: Props) => {
  const [worker, setWorker] = useState<WorkerGET | null>(null);
  const [loader, setLoader] = useState<boolean>(false);

  const handleGetWorker = async () => {
    setLoader(false);
    if(workerId != null) {
      const response = await WorkerGetAPI()
    }
    setLoader(true);
  }

  useEffect(() => {
handleGetWorker();
  },[])


  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div className="bg-white p-4 rounded-lg shadow-lg m-2 flex">

      </div>
    </div>
  )
}

export default InspectUserModal