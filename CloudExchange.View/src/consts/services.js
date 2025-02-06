class Service{

    constructor(name, description)
    {
        this.name = name;
        this.description = description;
    }

    name;
    description;
}

export class UploadService extends Service{
    name = "Upload";
    description = "You can upload files to our server with the option to set a password to download the file. You can also set the file lifetime on our server.";
}

export class DownloadService extends Service{
    name = "Download";
    description = "You can download files from our server, that have not expired."
}

export class DeleteService extends Service{
    name = "Delete";
    description = "You can delete your uploaded file from our server, which has not expired yet."
}

export const Services = [
    new UploadService(),
    new DownloadService(),
    new DeleteService(),
]