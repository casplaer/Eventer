import React, { useState } from "react";

const ImageUploader = () => {
    const [images, setImages] = useState([]);

    const handleImageUpload = (event, index) => {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = () => {
                const newImages = [...images];
                newImages[index] = reader.result;
                setImages(newImages);
                if (images.length < 3 && !newImages.includes(null)) {
                    setImages([...newImages, null]); // Добавляем новый слот только после загрузки изображения
                }
            };
            reader.readAsDataURL(file);
        }
    };

    return (
        <div style={{ display: "flex", gap: "10px" }}>
            {images.map((image, index) => (
                <div key={index} style={{ position: "relative" }}>
                    {image ? (
                        <img
                            src={image}
                            alt={`Uploaded ${index + 1}`}
                            style={{
                                width: "150px",
                                height: "150px",
                                objectFit: "cover",
                                borderRadius: "8px",
                            }}
                        />
                    ) : (
                        <label
                            htmlFor={`upload-${index}`}
                            style={{
                                width: "150px",
                                height: "150px",
                                background: "gray",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                cursor: "pointer",
                                borderRadius: "8px",
                            }}
                        >
                            +
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
            {/* Последний квадрат создаётся только если он пустой */}
            {images.length === 0 && (
                <div
                    style={{
                        width: "150px",
                        height: "150px",
                        background: "gray",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        cursor: "pointer",
                        borderRadius: "8px",
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
                        +
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
