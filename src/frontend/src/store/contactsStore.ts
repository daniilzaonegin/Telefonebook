import { Contact } from "@/models/contact";
import { defineStore } from "pinia";
import contactsApiClient from "@/services/contacts-api-client";
import axios, { AxiosError } from "axios";
import { LoginModel } from "@/models/login";
import { AuthInfo } from "@/models/authInfo";
import { registerAuthTokenInterceptor } from "@/services/http-common";

export const useContactsStore = defineStore("contacts", {
  state: () => ({
    contacts: [] as Contact[],
    error: "" as string,
  }),
  getters: {
    allContacts: (state) => state.contacts as Contact[],
    errorMessage: (state) => state.error,
  },
  actions: {
    async fetchContacts() {
      try {
        this.contacts = await contactsApiClient.getAll();
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const axiosError = error as AxiosError;
          this.error = axiosError.message;
        } else {
          const typedError = error as Error;
          this.error = typedError.message;
        }
        throw error;
      }
    },
    async addContact(newContact: Contact): Promise<Contact> {
      const existingContact = this.contacts.find(
        (c) => c.name == newContact.name && c.surname == newContact.name
      );

      if (existingContact) {
        throw new Error("Contact already exists!");
      }

      try {
        const createdContact = await contactsApiClient.createContact(
          newContact
        );
        this.contacts.push(createdContact);
        return createdContact;
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const axiosError = error as AxiosError;
          this.error = axiosError.message;
        } else {
          const typedError = error as Error;
          this.error = typedError.message;
        }
        throw new Error(this.errorMessage);
      }
    },
    async editContact(newContact: Contact): Promise<Contact> {
      if (!newContact?.id) {
        this.error = "Contact doesn't have the id!";
        throw new Error("Contact doesn't have the id!");
      }
      const existingContact = this.contacts.find((c) => c.id == newContact.id);
      if (!existingContact) {
        this.error = "Contact not found in the store!";
        throw new Error("Contact not found in the store!");
      }
      try {
        const editedContact = await contactsApiClient.editContact(
          newContact.id,
          newContact
        );

        existingContact.cellPhoneNumber = editedContact.cellPhoneNumber;
        existingContact.name = editedContact.name;
        existingContact.surname = editedContact.surname;
        existingContact.email = editedContact.email;
        existingContact.homePhoneNumber = editedContact.homePhoneNumber;

        return existingContact;
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const axiosError = error as AxiosError;
          this.error = axiosError.message;
        } else {
          const typedError = error as Error;
          this.error = typedError.message;
        }
        throw new Error(this.errorMessage);
      }
    },
    async removeContacts(contacts: Contact[]): Promise<void> {
      for (const contact of contacts) {
        if (this.contacts.find((c) => c.id == contact.id)) {
          try {
            await contactsApiClient.deleteContact(contact);
            //delete this contact from store
            this.contacts = this.contacts.filter((c) => c.id != contact.id);
          } catch (error) {
            if (axios.isAxiosError(error)) {
              const axiosError = error as AxiosError;
              this.error = axiosError.message;
            } else {
              const typedError = error as Error;
              this.error = typedError.message;
            }
            throw new Error(this.errorMessage);
          }
        } else {
          throw new Error(`Contact with id ${contact.id} not found in store`);
        }
      }
    },
  },
});
