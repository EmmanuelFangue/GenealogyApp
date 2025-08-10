import React, { useState } from "react";
import apiClient from "../api/apiClient";

export default function PhotoUpload({
  memberId,
  onUploadSuccess
}: {
  memberId: string;
  onUploadSuccess?: () => void;
})  {
  const [file, setFile] = useState<File | null>(null);
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);

  
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setFile(e.target.files[0]);
    }
  };
  const handleUpload = async (e: React.FormEvent) => {
      e.preventDefault();
      if (!file) return;
      setLoading(true);
      setSuccess(false);

      const formData = new FormData();
      formData.append("file", file);
      formData.append("memberId", memberId);

      try {
        await apiClient.post(`/Photo/${memberId}`, formData, {
          headers: { "Content-Type": "multipart/form-data" }
        });
        setSuccess(true);
        if (onUploadSuccess) onUploadSuccess();
      } catch (err) {
        alert("Erreur lors de l'upload.");
      } finally {
        setLoading(false);
    }
  };

  return (
    <div>
      <form onSubmit={handleUpload}>
        <input type="file" accept="image/*" onChange={handleFileChange} />
        <button type="submit" disabled={loading || !file}>Uploader la photo</button>
      </form>
      {success && <div>Upload réussi !</div>}
    </div>
  );
}
