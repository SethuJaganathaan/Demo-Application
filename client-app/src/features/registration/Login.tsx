import { Container, TextField, Button, Box } from "@mui/material";
import { useState } from "react";
import { Card, CardContent } from "semantic-ui-react";
import { Link, useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import agent from "../../app/api/agent";

interface FormValues {
    email: string;
    password: string;
}

export default function Login() {
    const navigate = useNavigate();

    const initialValues: FormValues = {
        email: "",
        password: "",
    };

    const [formValues, setFormValues] = useState<FormValues>(initialValues);

    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setFormValues({ ...formValues, [name]: value });
    };

    function handleSubmit() {
        agent.Application.login(formValues.email, formValues.password)
            .then((response) => {
                if (response.status === "LOGIN SUCCESSFUL") {
                    const token = response.token;
                    localStorage.setItem("token", token);
                    navigate("/dashboard");
                    toast.success(response.status);
                } 
                else {
                    toast.error(response.status);
                }
                console.log(response);
            })
            .catch((error) => {
                toast.error("Login Failed");
            });
    }

    return (
        <Container>
            <Box
                display="flex"
                justifyContent="center"
                alignItems="center"
                minHeight="100vh"
            >
                <Card sx={{ width: "200px", height: "400px" }}>
                    <CardContent>
                        <Box
                            component="span"
                            sx={{
                                fontWeight: "bold",
                                fontSize: "16px",
                                display: "block",
                                textAlign: "center",
                            }}
                        >
                            <b>Login</b>
                        </Box>
                        <form>
                            <TextField
                                id="email"
                                label="Email"
                                variant="outlined"
                                fullWidth
                                margin="normal"
                                name="email"
                                value={formValues.email}
                                onChange={handleChange}
                            />

                            <TextField
                                type="password"
                                id="password"
                                label="Password"
                                variant="outlined"
                                fullWidth
                                margin="normal"
                                name="password"
                                value={formValues.password}
                                onChange={handleChange}
                            />

                            <Button
                                variant="contained"
                                color="primary"
                                fullWidth
                                onClick={handleSubmit}
                            >
                                Login
                            </Button>
                        </form>
                        <h4>
                            create an account <Link to="/signup">signup</Link>
                        </h4>
                    </CardContent>
                </Card>
            </Box>
            <ToastContainer
                position="top-right"
                hideProgressBar
                theme="colored"
            />
        </Container>
    );
}
