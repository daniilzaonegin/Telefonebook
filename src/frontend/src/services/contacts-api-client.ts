import { AuthData } from "@/models/authToken";
import { Contact } from "@/models/contact";
import { LoginModel } from "@/models/login";
import http from "@/services/http-common";

class ContactsApiClient {
  async getAll(): Promise<Contact[]> {
    const response = await http.get("/api/contacts");
    return response.data as Contact[];
  }
  async getById(id: Number): Promise<Contact> {
    const response = await http.get(`/api/contacts/${id}`);
    return response.data as Contact;
  }
  async createContact(newContact: Contact): Promise<Contact> {
    const response = await http.post("/api/contacts", newContact);
    return response.data as Contact;
  }
  async editContact(id: Number, newContact: Contact): Promise<Contact> {
    const response = await http.post(`/api/contacts/edit/${id}`, newContact);
    return response.data as Contact;
  }
  async deleteContact(contactToDelete: Contact): Promise<void> {
    await http.delete(`/api/contacts/${contactToDelete.id}`);
  }

  async authenticate(authData: LoginModel): Promise<AuthData> {
    const response = await http.post(`/api/token`, authData);
    return response.data as AuthData;
  }
  
  async register(user: LoginModel): Promise<void> {
    await http.post(`/api/token/register/user`, user);
  }
}

export default new ContactsApiClient();
