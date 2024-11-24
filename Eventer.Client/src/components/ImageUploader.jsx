import React, { useState } from "react";
import plus from "../assets/plus.png"

const ImageUploader = () => {
    const [images, setImages] = useState([]);

    const handleImageUpload = (event, index) => {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = () => {
                const fileDataUrl = reader.result;

                if (images.includes(fileDataUrl)) {
                    alert("Это изображение уже добавлено!");
                    return;
                }

                const newImages = [...images];
                newImages[index] = fileDataUrl;
                setImages(newImages);

                if (newImages.length < 3 && !newImages.includes(null)) {
                    setImages([...newImages, null]);
                }
            };
            reader.readAsDataURL(file);
        }
    };

    const handleRemoveImage = (index) => {
        const newImages = [...images];
        newImages.splice(index, 1);
        setImages(newImages);
    };

    return (
        <div style={{ display: "flex", gap: "10px" }}>
            {images.map((image, index) => (
                <div
                    key={index}
                    style={{
                        position: "relative",
                        width: "150px",
                        height: "150px",
                    }}
                >
                    {image ? (
                        <>
                            <img
                                src={image}
                                alt={`Uploaded ${index + 1}`}
                                style={{
                                    width: "100%",
                                    height: "100%",
                                    objectFit: "cover",
                                    borderRadius: "8px",
                                }}
                            />
                            <button
                                onClick={() => handleRemoveImage(index)}
                                style={{
                                    position: "absolute",
                                    top: "5px",
                                    right: "5px",
                                    background: "black",
                                    color: "white",
                                    border: "none",
                                    borderRadius: "0%",
                                    width: "20px",
                                    height: "20px",
                                    display: "flex",
                                    alignItems: "center",
                                    justifyContent: "center",
                                    cursor: "pointer",
                                }}
                            >
                                ×
                            </button>
                        </>
                    ) : (
                        <label
                            htmlFor={`upload-${index}`}
                            style={{
                                width: "150px",
                                height: "150px",
                                background: "lightgray",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                cursor: "pointer",
                                borderRadius: "4px",
                                fontSize: "35px",
                            }}
                        >
                            <img
                                src={plus}
                                alt="Add"
                                style={{ width: "25px", height: "25px" }}
                            />
                            <input
                                id={`upload-${index}`}
                                type="file"
                                accept="image/*"
                                style={{ display: "none" }}
                                onChange={(e) => handleImageUpload(e, index)}
                            />
                        </label>
                    )}
                </div>
            ))}
            {images.length === 0 && (
                <div
                    style={{
                        width: "150px",
                        height: "150px",
                        background: "lightgray",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        cursor: "pointer",
                        borderRadius: "4px",
                        fontSize: "35px",
                    }}
                >
                    <label
                        htmlFor="upload-0"
                        style={{
                            width: "100%",
                            height: "100%",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            cursor: "pointer",
                        }}
                    >
                        <img
                            src={ plus }
                            alt="Add"
                            style={{ width: "25px", height: "25px" }}
                        />
                        <input
                            id="upload-0"
                            type="file"
                            accept="image/*"
                            style={{ display: "none" }}
                            onChange={(e) => handleImageUpload(e, 0)}
                        />
                    </label>
                </div>
            )}
        </div>
    );
};

export default ImageUploader;
