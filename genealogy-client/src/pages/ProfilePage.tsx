import { useContext, useEffect, useState } from "react";
import { AuthContext } from "../contexts/AuthContext";
import { getUser } from "../api/userApi";
import { getPhotos } from "../api/photoApi";
import { setProfilePhoto } from "../api/photoApi";
import PhotoUpload from "../components/PhotoUpload";

export default function ProfilePage() {
  const context = useContext(AuthContext);
  const [userDetails, setUserDetails] = useState<any>(null);
  const [photos, setPhotos] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      if (!context?.user?.id) return;

      try {
        const user = await getUser(context.user.id);
        setUserDetails(user);

        // Si l'utilisateur a un membre principal (lui-même), on récupère ses photos
        if (user.profileMemberId) {
          const memberPhotos = await getPhotos(user.profileMemberId);
          setPhotos(memberPhotos);
        }
      } catch (err) {
        console.error("Erreur lors du chargement du profil :", err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [context]);

  if (loading) return <div>Chargement du profil...</div>;

  return (
    <div style={{ padding: "2rem" }}>
      <h2>Mon profil</h2>
      {userDetails ? (
        <>
          <p><strong>Nom d'utilisateur :</strong> {userDetails.username}</p>
          <p><strong>Email :</strong> {userDetails.email}</p>
          <p><strong>Téléphone :</strong> {userDetails.phoneNumber ?? "Non renseigné"}</p>

          <h3>Mes photos</h3>
          <h3>Ajouter une photo</h3>
          <PhotoUpload memberId={userDetails.profileMemberId}
                    onUploadSuccess={() => {
                        // Recharge les photos après upload
                        getPhotos(userDetails.profileMemberId).then(setPhotos);
                    }}
                    />


          {photos.map((photo) => (
            <div key={photo.photoId} style={{ position: "relative" }}>
            <img src={photo.url} alt="Photo" width={150} height={150} />
            <button
            onClick={() => {
                setProfilePhoto(userDetails.profileMemberId, photo.photoId)
                .then(() => {
                    // Recharger les infos utilisateur
                    if(context?.user?.id)
                    {
                        getUser(context.user.id).then(setUserDetails);
                    }
                })
                .catch(() => alert("Erreur lors de la mise à jour de la photo de profil."));
            }}
            style={{ position: "absolute", top: 5, right: 5 }}
            >
            Définir comme photo de profil
            </button>
        </div>
        ))}

        </>
      ) : (
        <p>Profil introuvable.</p>
      )}
    </div>
  );
}
